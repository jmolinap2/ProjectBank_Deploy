using System.ComponentModel.DataAnnotations;

namespace ProjectBlack.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}