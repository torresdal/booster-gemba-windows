using ConDep.Dsl;

namespace Booster.ConDep
{
    public static class GembaExtensions
    {
        public static IOfferRemoteConfiguration Acl(this IOfferRemoteConfiguration remote, string user, string path)
        {
            var op = new SetAclOperation(user, path);
            Configure.Operation(remote, op);
            return remote;
        }    
    }
}