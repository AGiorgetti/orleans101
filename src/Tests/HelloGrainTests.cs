using GrainInterfaces;
using NUnit.Framework;
using Orleans.Hosting;
using Orleans.TestingHost;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class HelloGrainTests
    {
        public TestCluster Cluster { get; private set; }

        public class TestSiloConfigurations : ISiloBuilderConfigurator
        {
            public void Configure(ISiloHostBuilder hostBuilder)
            {
                hostBuilder.ConfigureServices(services => {
                    // configure the inversion of control here!
                });
            }
        }

        [OneTimeSetUp]
        public void OneTImeSetup()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
            Cluster = builder.Build();
            Cluster.Deploy();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Cluster.StopAllSilos();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SayHelloWorks()
        {
            var hello = Cluster.GrainFactory.GetGrain<IHello>("TestGrain_1");
            var greeting = await hello.SayHello("Test");

            Assert.AreEqual("\n Client said: 'Test', so HelloGrain says: Hello!", greeting);
        }
    }
}