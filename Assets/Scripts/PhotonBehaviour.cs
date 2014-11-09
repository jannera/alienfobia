using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompleteProject
{
    public class PhotonBehaviour : Photon.MonoBehaviour
    {
        public void RPC(Action method, PhotonTargets target)
        {
            doRPC(method.Method.Name, target);
        }

        public void RPC<T>(Action<T> method, PhotonTargets target, T parameters)
        {
            doRPC(method.Method.Name, target, parameters);
        }

        public void RPC<T1, T2>(Action<T1, T2> method, PhotonTargets target, T1 t1, T2 t2)
        {
            doRPC(method.Method.Name, target, t1, t2);
        }

        public void RPC<T1, T2, T3>(Action<T1, T2, T3> method, PhotonTargets target, T1 t1, T2 t2, T3 t3)
        {
            doRPC(method.Method.Name, target, t1, t2, t3);
        }

        public void RPC<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, PhotonTargets target, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            doRPC(method.Method.Name, target, t1, t2, t3, t4);
        }

        private void doRPC(String name, PhotonTargets target, params object[] parameters)
        {
            photonView.RPC(name, target, parameters);
        }

        public void Invoke(Action method, float time)
        {
            Invoke(method.Method.Name, time);
        }
    }
}
