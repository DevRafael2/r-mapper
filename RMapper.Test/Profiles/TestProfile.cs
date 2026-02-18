using RMapper.Test.Models;

namespace RMapper.Test.Profiles;

using Configurations;

/// <summary>
/// Perfil test.
/// </summary>
public class TestProfile : Profile
{
    /// <summary>
    /// Constructor donde se indican reglas.
    /// </summary>
    public TestProfile()
    {
        CreateMap<InEntity, OutEntity>();
    }
}