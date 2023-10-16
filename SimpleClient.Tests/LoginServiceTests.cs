using SimpleClient.Login;
using SimpleClient.Login.ClientServices;
using Moq;
using Models.dto;

namespace SimpleClient.Tests
{
    public class Tests
    {
        private LoginService service;
        private IClientLoginService clientService;

        [SetUp]
        public void Setup()
        {
            clientService = Mock.Of<IClientLoginService>();
            service = new LoginService(clientService);
        }

        [Theory]
        [TestCase(null, null)]
        [TestCase(null, "any")]
        [TestCase("", "any")]
        [TestCase("any", null)]
        [TestCase("any", "")]
        [Test]
        public void TestLoginAsync_WhenRequestInvalid_ShouldFailedToLogin(string user, string password)
        {
            var request = new LoginReqProto() { Password = password, User= user, InSeqNum=1};

            var result = service.LoginAsync(request).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsOk, Is.False);
        }

        [Test]
        public void TestLoginAsync_WhenRequestValid_ShouldSuccess()
        {
            var request = new LoginReqProto() { Password = "any", User = "any", InSeqNum = 1 };
            var cancellationToken = CancellationToken.None;
            Mock.Get(clientService).Setup(x => x.LoginAsync(request, cancellationToken)).Returns(Task.FromResult(new LoginRespProto() { IsOk=true}));
           
            var result = service.LoginAsync(request).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsOk, Is.True);
        }

        [Test]
        public void TestDispose()
        {
            service.Dispose();
            Mock.Get(clientService).Verify(x => x.Dispose());
        }
    }
}