using AutoMapper;
using Grpc.Net.Client;
using Services.CommandService.Models;
using Services.PlatformService;

namespace Services.CommandService.SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration,IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        string address = _configuration["GrpcPlatform"];
        Console.WriteLine($"--> Calling GRPC Service {address}");
        var channel = GrpcChannel.ForAddress(address);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();
        try
        {
            var reply=client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
            return null;
        }
    }
}
