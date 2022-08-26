using FluentValidation;


// para validaciones
namespace CleanArchitecture.Application.Features.Streamers.Commands
{
    // Usamos el AbstractValidator que viene de FluentValidator
    public class CreateStreamerCommandValidator : AbstractValidator<CreateStreamerCommand>
    {
        //constructor por defecto
        public CreateStreamerCommandValidator()
        {
            //Reglas de validación
            RuleFor(p => p.Nombre)
                    .NotEmpty().WithMessage("{Nombre} no puede estar en blanco")
                    .NotNull();

            //hay un maximum lenght que no he podido usar


            RuleFor(p => p.Url)
                   .NotEmpty().WithMessage("{Nombre} no puede estar en blanco");


        }

    }
}
