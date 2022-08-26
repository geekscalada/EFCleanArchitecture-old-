using AutoMapper;
using CleanArchitecture.Application.Contracts.Infraestructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Application.Features.Streamers.Commands
{
    public class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommand, int>
    {

        // Aquí lo que estamos haciendo es inyectar los objetos dentro de la clase
        // Son servicios que vamos a usar en la clase

        private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
       // Este es un servicio de log, que va a trabajar sobre esta clase, StreamerCommandHandler
        private readonly ILogger<CreateStreamerCommandHandler> _logger;

        public CreateStreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper, IEmailService emailService, ILogger<CreateStreamerCommandHandler> logger)
        {
            _streamerRepository = streamerRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }


        //Constructor, con el ctrol .

        //este handle es obligatorio, una vez hayamos creado el constructor, habiendo antes
        //inyectado lo necesario, podremos trabajar con esos objetos
        public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
        {

             // lo que nos da el cliente es un StreamerCommand, pero lo que nosotros
             // insertaremos en la BDD es un Streamer, por ello tenemos que mapearlo con el IMapper
             // recuerda que request es de tipo Streamercomand, que solo mapea dos propiedades string

            var streamerEntity = _mapper.Map<Streamer>(request);

            var newStreamer = await _streamerRepository.AddAsync(streamerEntity);

            _logger.LogInformation($"Streamer {newStreamer.Id} fue creado con éxito");


            //Enviamos mail
            await SendEmail(newStreamer);

            // este return lo necesitamos porque estamos devolviendo un Task(int)
            return newStreamer.Id;
            
        }

        // Método para enviar el email
        // Como no va a ser usado fuera de aquí, lo declaramos como try catch
        private async Task SendEmail(Streamer streamer)
        {
            var email = new Email
            {
                To = "jescalada87@gmail.com"
                ,
                Subject = "Hola desde ASP.net"
                ,
                Body = "La compañia se creó correctamente"
            };


            try {

                await _emailService.SendEmail(email);

            } catch {

                _logger.LogError($"Errores enviando el email de {streamer.Id}");
            }
            
        }
    }
}
