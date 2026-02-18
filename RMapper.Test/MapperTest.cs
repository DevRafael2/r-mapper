using RMapper.Configurations;
using RMapper.Implementation;
using RMapper.Interfaces;
using RMapper.Test.Profiles;

namespace RMapper.Test;

[TestClass]
public sealed class MapperTest
{
    /// <summary>
    /// RMapper.
    /// </summary>
    private IMapper Mapper { get; set; }
    
    public MapperTest()
    {
        Mapper = new Mapper();
        var profile = new TestProfile();
        profile.
    }
    
    [TestMethod]
    public void SimpleMapper()
    {
        
    }
}