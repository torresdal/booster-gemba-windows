using System;
using ConDep.Dsl;
using ConDep.Dsl.Config;

namespace Booster.ConDep
{
    public class BootstrapWebServers : Artifact.Local
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepSettings settings)
        {
        }
    }
}
