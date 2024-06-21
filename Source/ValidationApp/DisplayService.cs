using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using System;

namespace Validation;

public class DisplayService
{
    public event EventHandler<TestResult>? ResultClicked;

    private Label _testNameLabel;
    private Label _questionLabel;
    private Label _instructionLabel;
    private Label _inputsLabel;
    private Button _yesButton;
    private Button _noButton;
    private Button _skipButton;

    private AbsoluteLayout _testLayout;
    private AbsoluteLayout _resultsLayout;
    private Label _resultPassLabel;
    private Label _resultFailLabel;
    private Label _resultSkipLabel;

    public DisplayScreen Screen { get; }

    public DisplayService(DisplayScreen screen)
    {
        Screen = screen;
    }

    public void CreateValidationControls()
    {
        Screen.Controls.Clear();
        Screen.BackgroundColor = Color.Black;

        var buttonFont = new Font16x24();

        _testLayout = new AbsoluteLayout(0, 0, Screen.Width, Screen.Height);
        _testNameLabel = new Label(0, 0, Screen.Width, buttonFont.Height + 4)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font12x16(),
            TextColor = Color.Yellow
        };

        _questionLabel = new Label(0, buttonFont.Height * 2, Screen.Width, buttonFont.Height + 4)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font8x12(),
            TextColor = Color.White
        };

        _instructionLabel = new Label(0, buttonFont.Height * 3, Screen.Width, buttonFont.Height + 4)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font8x12(),
            TextColor = Color.White
        };

        _inputsLabel = new Label(0, buttonFont.Height * 4, Screen.Width, buttonFont.Height + 4)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font8x12(),
            TextColor = Color.Yellow
        };

        var buttonHeight = 50;
        var buttonWidth = 100;

        _yesButton = new Button(
            0,
            Screen.Height - buttonHeight - 2,
            buttonWidth,
            buttonHeight)
        {
            Font = buttonFont,
            Text = "YES",
            HighlightColor = Color.White,
            ForeColor = Color.LightGray,
            PressedColor = Color.DarkGray
        };
        _yesButton.Clicked += (s, e) => { ResultClicked?.Invoke(null, TestResult.Pass); };

        _skipButton = new Button(
            (Screen.Width - buttonWidth) / 2,
            Screen.Height - buttonHeight - 2,
            buttonWidth,
            buttonHeight)
        {
            Font = buttonFont,
            Text = "Skip",
            HighlightColor = Color.White,
            ForeColor = Color.LightGray,
            PressedColor = Color.DarkGray
        };
        _skipButton.Clicked += (s, e) => { ResultClicked?.Invoke(null, TestResult.Skip); };

        _noButton = new Button(
            Screen.Width - buttonWidth - 2,
            Screen.Height - buttonHeight - 2,
            buttonWidth,
            buttonHeight)
        {
            Font = buttonFont,
            Text = "No",
            HighlightColor = Color.White,
            ForeColor = Color.LightGray,
            PressedColor = Color.DarkGray
        };
        _noButton.Clicked += (s, e) => { ResultClicked?.Invoke(null, TestResult.Fail); };
        _testLayout.Controls.Add(_testNameLabel, _questionLabel, _instructionLabel, _inputsLabel, _yesButton, _noButton, _skipButton);

        _resultsLayout = new AbsoluteLayout(0, 0, Screen.Width, Screen.Height)
        {
            IsVisible = false
        };

        _resultPassLabel = new Label(0, 0, Screen.Width, buttonFont.Height)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font12x16(),
            TextColor = Color.Green,
            Text = "[N] passed"
        };
        _resultFailLabel = new Label(0, _resultPassLabel.Bottom + 2, Screen.Width, buttonFont.Height)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font12x16(),
            TextColor = Color.Red,
            Text = "[N] failed"
        };
        _resultSkipLabel = new Label(0, _resultFailLabel.Bottom + 2, Screen.Width, buttonFont.Height)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = new Font12x16(),
            TextColor = Color.Yellow,
            Text = "[N] skipped"
        };

        _resultsLayout.Controls.Add(_resultPassLabel, _resultFailLabel, _resultSkipLabel);

        Screen.Controls.Add(_testLayout, _resultsLayout);

        _resultsLayout.IsVisible = false;
    }

    public void ShowResults(int passCount, int failCount, int skipCount)
    {
        _resultPassLabel.Text = $"{passCount} passed";
        _resultFailLabel.Text = $"{failCount} failed";
        _resultSkipLabel.Text = $"{skipCount} skipped";

        _testLayout.IsVisible = false;
        _resultsLayout.IsVisible = true;
    }

    public void ClearTestLabels()
    {
        _testNameLabel.Text = string.Empty;
        _questionLabel.Text = string.Empty;
        _instructionLabel.Text = string.Empty;
        _inputsLabel.Text = string.Empty;
    }

    public void SetTestName(string name)
    {
        _testNameLabel.Text = name;
    }

    public void SetInstructionText(string text)
    {
        _instructionLabel.Text = text;
    }

    public void SetQuestionText(string text)
    {
        _questionLabel.Text = text;
    }

    public void SetInputsLabel(string text)
    {
        _inputsLabel.Text = text;
    }
}
