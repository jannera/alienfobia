using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    public abstract class Weapon : CompleteProject.PhotonBehaviour
    {
        public int clipSize;
        public int currentAmmo;
        public Sprite ammoSprite;

        public abstract bool IsReloading();

        public abstract float ReloadReadiness();

        public abstract bool CanFire();

        public abstract bool HasInfiniteClips();

        public void ReceiveAmmo(int increase)
        {
            currentAmmo += increase;
            if (currentAmmo > clipSize)
            {
                currentAmmo = clipSize;
            }
        }
    }
}
