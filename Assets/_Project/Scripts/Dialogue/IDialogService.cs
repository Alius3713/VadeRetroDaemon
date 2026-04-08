using System;

namespace _Project.Scripts.Dialogue
{
    public interface IDialogService
    {
        void PlayDialog(string dialogId, Action onDialogFinished = null);
        bool IsDialogPlaying { get; }
    }
}
