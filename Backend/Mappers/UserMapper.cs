using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Dtos.UserDtos;
using CourseHouse.Models;
using CoursesHouse.Mappers;

namespace Backend.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Emial = user.Email,
                Roles = new List<string>(),
                UserPurchases = user.UserPurchases,
                UserCreditCards = user.UserCreditCards,
                CreatedCourses = user.CreatedCourses.Select(a => a.ToCourseDto()).ToList(),
                Comments = user.Comments.Select(a => a.ToCommentDto()).ToList(),
                Grades = user.Grades.Select(a => a.ToGradeDto()).ToList()
            };
        }

        public static User ToUser(this UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                Email = userDto.Emial,
            };
        }

        public static User ToUserFromUserUpdatedDto(this UserUpdateDto userUpdateDto)
        {
            return new User
            {
                UserName = userUpdateDto.UserName,
                Email = userUpdateDto.Email
            };
        }
    }
}