using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Domain.Entities;

// Add profile data for application users by adding properties to the GymUser class

public class GymUser : IdentityUser
{
    [PersonalData]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [PersonalData]
    [MaxLength(100)]
    public string LastName { get; set; }

    public ICollection<Training> Trainings { get; set; }
}

