using System;
using ConDep.Dsl;
using ConDep.Dsl.Config;

namespace Booster.ConDep
{
    public class BootstrapWebServers : Artifact.Local
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepSettings settings)
        {
            onLocalMachine.Aws(aws => aws
                .Ec2.CreateInstances("booster-gemba-01", opt => opt
                    .Image.LatestBaseWindowsImage(AwsWindowsImage.Win2012R2)
                    .InstanceCount(2, 2)
                    .InstanceType(AwsInstanceType.T2.Small)
                    .SecurityGroupIds("sg-60c5b605")
                    .SubnetId("subnet-ca6fd0bd")
                )
                .Elb.CreateLoadBalancer("booster-lb-01", opt => opt
                    .Listeners.Add(AwsElbProtocol.Http, 80, AwsElbProtocol.Http, 80)
                    .SecurityGroups("sg-60c5b605")
                    .Subnets("subnet-1c17c745", "subnet-60fe6805", "subnet-ca6fd0bd")
                )
            );
        }
    }

    public class ConfigureNodeJs : Artifact.Remote, IDependOn<BootstrapWebServers>
    {
        public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
        {
            server.Configure
                .IIS(opt => opt.Include.AspNet45().AspNet45Ext());

            server.Install
                .Chocolatey("urlrewrite")
                .Chocolatey("nodejs")
                .Msi("iisnode for iis 7.x (x64) full", new Uri("https://github.com/tjanczuk/iisnode/releases/download/v0.2.16/iisnode-full-v0.2.16-x64.msi"));
        }
    }

    public class Deploy : Artifact.Remote, IDependOn<ConfigureNodeJs>
    {
        public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
        {
            const string appDir = @"C:\temp\web";

            server.Deploy.Directory(@"web", appDir);
            server.Configure.Acl("IIS_IUSRS", appDir);
            server.Configure.IISWebApp("gemba", "Default Web Site", opt => opt.PhysicalPath(appDir));

        }
    }
}
