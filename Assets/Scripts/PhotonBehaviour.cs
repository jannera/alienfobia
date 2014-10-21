using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompleteProject
{
    public class PhotonBehaviour : Photon.MonoBehaviour
    {
        public void RPC(Action method, PhotonTargets target, params object[] parameters)
        {
            doRPC(method.Method.Name, target, parameters);
        }

        public void RPC<T>(Action<T> method, PhotonTargets target, params object[] parameters)
        {
            doRPC(method.Method.Name, target, parameters);
        }

        public void RPC<T1, T2>(Action<T1, T2> method, PhotonTargets target, params object[] parameters)
        {
            doRPC(method.Method.Name, target, parameters);
        }

        private void doRPC(String name, PhotonTargets target, params object[] parameters)
        {
            photonView.RPC(name, target, parameters);
        }
    }
}
