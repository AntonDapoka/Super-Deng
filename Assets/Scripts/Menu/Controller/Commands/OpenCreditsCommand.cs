using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCreditsCommand : Command
{
    [SerializeField] private OpenCreditsInteractorScript interactor;

    public override void Execute()
    {
        _ = interactor.OpenCreditsAsync();
    }
}