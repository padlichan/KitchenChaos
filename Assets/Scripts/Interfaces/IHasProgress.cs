using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgresschangeEventArgs> OnProgressChange;
    public class OnProgresschangeEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
