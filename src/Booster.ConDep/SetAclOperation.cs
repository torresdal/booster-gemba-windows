using ConDep.Dsl;
using ConDep.Dsl.Validation;

namespace Booster.ConDep
{
    public class SetAclOperation : RemoteCompositeOperation
    {
        private readonly string _user;
        private readonly string _path;

        public SetAclOperation(string user, string path)
        {
            _user = user;
            _path = path;
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override string Name
        {
            get { return "Set ACL"; }
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            server.Execute.PowerShell(string.Format(@"
$Acl = Get-Acl ""{0}""
$Ar = New-Object  system.security.accesscontrol.filesystemaccessrule(""{1}"",""FullControl"", ""ContainerInherit, ObjectInherit, None"", ""InheritOnly"",""Allow"")
$Acl.SetAccessRule($Ar)
Set-Acl ""{0}"" $Acl
            ", _path, _user));
        }
    }
}