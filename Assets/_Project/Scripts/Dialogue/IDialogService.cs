using System;

namespace _Project.Scripts.Dialogue
{
    public interface IDialogService
    {
        void PlayDialog(string dialogId, Action onDialogEnded = null);
        bool IsDialogPlaying { get; }
    }
}
