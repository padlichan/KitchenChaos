using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgresschangeEventArgs> OnProgressChanged;
    public class OnProgresschangeEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
