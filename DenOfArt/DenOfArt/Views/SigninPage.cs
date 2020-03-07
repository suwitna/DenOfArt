using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DenOfArt.Views
{
    public class SigninPage : ContentPage
    {
        private Label headerLabel;
        private Entry emailEntry;
        private Entry passwordEntry;
        private Button signinButton;

        public SigninPage()
        {
            StackLayout stackLayout = new StackLayout();

            headerLabel = new Label();
            headerLabel.Text = "Login Page";
            headerLabel.FontAttributes = FontAttributes.Bold;
            headerLabel.Margin = new Thickness(10, 10, 10, 10);
            headerLabel.HorizontalOptions = LayoutOptions.StartAndExpand;
            stackLayout.Children.Add(headerLabel);

            emailEntry = new Entry();
            emailEntry.Keyboard = Keyboard.Email;
            emailEntry.Placeholder = "Email ID";
            stackLayout.Children.Add(emailEntry);

            passwordEntry = new Entry();
            passwordEntry.Keyboard = Keyboard.Text;
            passwordEntry.Placeholder = "Password";
            passwordEntry.IsPassword = true;
            stackLayout.Children.Add(passwordEntry);

            signinButton = new Button();
            signinButton.Text = "Login";
            signinButton.Clicked += SigninButton_Clicked;
            stackLayout.Children.Add(signinButton);

            Content = stackLayout;
        }

        private void SigninButton_Clicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;
        }
    }
}