

namespace MauiAppFilterPanel;

public partial class MainPage : ContentPage
{
	int count = 0;

	public KeyValuePair<string, List<string>> init; 

	public MainPage()
	{
        InitializeComponent();
        List<string> list = new List<string>();
        for (int i = 0; i <= 10; i++)
        {
            list.Add(i.ToString() + "a");
        }
        var t = new Dictionary<string, string[]>();
        t.Add("Names", list.ToArray());
        t.Add("Famels", list.ToArray());
        Test.FiltredFilds = t;

       
		
        //ExpandList = new Controls.ExpandableList("Test", list);

    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

