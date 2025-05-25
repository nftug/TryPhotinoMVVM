using Photino.NET;

namespace TryPhotinoMVVM.Views;

public class PhotinoWindowInstance
{
    private PhotinoWindow? _window;

    public void Inject(PhotinoWindow window) => _window = window;

    public PhotinoWindow? Value => _window;
}
