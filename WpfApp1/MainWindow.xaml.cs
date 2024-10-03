using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the application
            this.Close();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCipher = (
                CipherComboBox.SelectedItem as ComboBoxItem
            )?.Content.ToString();
            string inputText = InputTextBox.Text.ToUpper();
            string key = KeyTextBox.Text.ToUpper();
            string result = "";

            switch (selectedCipher)
            {
                case "ROT13":
                    result = new ROT13Cipher().Encrypt(inputText);
                    break;
                case "Cézarova":
                    if (int.TryParse(key, out int shift))
                    {
                        result = new CaesarCipher(shift).Encrypt(inputText);
                    }
                    else
                    {
                        MessageBox.Show("Pro Cézarovu šifru zadejte platný číselný klíč.");
                    }
                    break;
                case "Vigenere":
                    result = new VigenereCipher(key).Encrypt(inputText);
                    break;
                default:
                    MessageBox.Show("Vyberte šifru.");
                    break;
            }

            OutputTextBox.Text = result;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCipher = (
                CipherComboBox.SelectedItem as ComboBoxItem
            )?.Content.ToString();
            string inputText = InputTextBox.Text.ToUpper();
            string key = KeyTextBox.Text.ToUpper();
            string result = "";

            switch (selectedCipher)
            {
                case "ROT13":
                    result = new ROT13Cipher().Decrypt(inputText);
                    break;
                case "Cézarova":
                    if (int.TryParse(key, out int shift))
                    {
                        result = new CaesarCipher(shift).Decrypt(inputText);
                    }
                    else
                    {
                        MessageBox.Show("Pro Cézarovu šifru zadejte platný číselný klíč.");
                    }
                    break;
                case "Vigenere":
                    result = new VigenereCipher(key).Decrypt(inputText);
                    break;
                default:
                    MessageBox.Show("Vyberte šifru.");
                    break;
            }

            OutputTextBox.Text = result;
        }
    }

    public class ROT13Cipher : ICipher
    {
        public string Encrypt(string text) => Transform(text);

        public string Decrypt(string text) => Transform(text);

        private string Transform(string text)
        {
            return new string(
                text.Select(c => char.IsLetter(c) ? (char)((c - 'A' + 13) % 26 + 'A') : c).ToArray()
            );
        }
    }

    public class CaesarCipher : ICipher
    {
        private int shift;

        public CaesarCipher(int shift) => this.shift = shift;

        public string Encrypt(string text) => Transform(text, shift);

        public string Decrypt(string text) => Transform(text, 26 - shift);

        private string Transform(string text, int shift)
        {
            return new string(
                text.Select(c => char.IsLetter(c) ? (char)((c - 'A' + shift) % 26 + 'A') : c)
                    .ToArray()
            );
        }
    }

    public class VigenereCipher : ICipher
    {
        private string keyword;

        public VigenereCipher(string keyword) => this.keyword = keyword.ToUpper();

        public string Encrypt(string text) => Transform(text, true);

        public string Decrypt(string text) => Transform(text, false);

        private string Transform(string text, bool encrypt)
        {
            int keywordIndex = 0;
            return new string(
                text.Select(c =>
                    {
                        if (!char.IsLetter(c))
                            return c;
                        int keyShift = keyword[keywordIndex % keyword.Length] - 'A';
                        keyShift = encrypt ? keyShift : 26 - keyShift;
                        keywordIndex++;
                        return (char)((c - 'A' + keyShift) % 26 + 'A');
                    })
                    .ToArray()
            );
        }
    }

    public interface ICipher
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}
