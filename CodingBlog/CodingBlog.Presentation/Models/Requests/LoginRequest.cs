using FluentValidation;

namespace CodingBlog.Presentation.Controllers.Requests;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password).NotEmpty();
    }
}