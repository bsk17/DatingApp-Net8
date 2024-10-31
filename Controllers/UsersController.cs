using System.Security.Claims;
using AutoMapper;
using DatingAppServer.DTOs;
using DatingAppServer.Entities;
using DatingAppServer.Extensions;
using DatingAppServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppServer.Controllers;

/// <summary>
/// UsersController uses the properties of BaseApiController created manually.
/// The route to this controller will be /api/users which is defined in BaseApiController
/// </summary>

[Authorize]
public class UsersController(IUserRepository userRepository,
    IMapper mapper,
    IPhotoService photoService) : BaseApiController
{
    /*
    [AllowAnonymous] // By default allow anonymous is set, But in case at top top level if it is set as Authorize then we can specifically use allow anonymous to overrider that.
    */
    [HttpGet] // /api/users
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")] // /api/users/lisa
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if (user == null) return NotFound();
        return Ok(user);
    }


    /// <summary>
    /// UpdateUser will help in persisting the changes made from angular side
    /// since we are sending the token along with the request from angular side
    /// therefore we can use the ClaimTypes.NameIdentifier to get the username 
    /// as it has been set in TokenService.cs (line 26 & 27)
    /// </summary>
    /// <param name="memberUpdateDTO">parameter is sent as json body from angular side</param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Could not find user!!");

        //make sure that the details from memberUpdateDTO is mapped into the user object that is just received from DB
        //NOTE:- This can also be done manually by individually assigning each values one by one
        mapper.Map(memberUpdateDTO, user);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the User!!!");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Cannot update user");
        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);

        //Simply returning photoDTO will create a 200 OK response
        //Instead we need to return 201 created and a location header to GetUser action of our controller, which can be directly accessed from response.
        if (await userRepository.SaveAllAsync())
        {
            /*
            Normally this would have returned
                return mapper.Map<PhotoDto>(photo);
            but instead
            */

            //GetUser action takes username as an attribute
            return CreatedAtAction(
                nameof(GetUser),
                new { username = user.UserName },
                mapper.Map<PhotoDto>(photo)
            );
        }

        return BadRequest("Problem adding photo!!");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Could not find user !!!");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo !!!");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("User not found !!!");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null) return BadRequest("This photo cannot be deleted !!!");

        if (photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo !!!");
    }
}
