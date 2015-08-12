using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using UTHPortal.Common;
using Windows.ApplicationModel.Email;

namespace UTHPortal.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private IDataService dataService;

        private string _description = "Το UTHPortal ειναι μια ιδέα που σχεδιάστηκε και υλοποιήθηκε απο φοιτητές του Π.Θ έτσι ώστε να προσφέρει στην πανεπιστημιακή κοινότητα. Στόχος της εφαρμογής ειναι η άμεση και εύκολη πρόσβαση σε πληροφορίες που συχνά χρειάζονται οι φοιτητές.\n\n Σε πρώτη φάση η εφαρμογή παρέχει πληροφορίες σχετικά με το πανεπιστήμιο και το τμήμα ΗΜΜΥ/ΤΜΗΥΔ. Πιστεύουμε πως υπάρχει μια πληθώρα δυνατοτήτων που μπορούν να προστεθούν σε μεταγενέστερες εκδόσεις, ωστόσο χρειαζόμαστε και τα δικά σας σχόλια για το μέλλον της εφαρμογής.\n\n Για οποιαδήποτε παρατήρηση/πρόβλημα/πρόταση μπορείτε να επικοινωνήσετε μαζί μας στο uthportal@gmail.com";

        /// <summary>
        /// Project full description string.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        public AboutViewModel()
        {
            if (!IsInDesignMode) {
                dataService = SimpleIoc.Default.GetInstance<IDataService>();
            }
        }

        /// <summary>
        /// Opens the rate & review box.
        /// </summary>
        public RelayCommand RateAppCommand
        {
            get
            {
                return _rateAppCommand
                    ?? (_rateAppCommand = new RelayCommand(
                        async () =>
                        {
                            await Windows.System.Launcher.LaunchUriAsync(
                              new Uri("zune:reviewapp?appid=appce084dc9-99d5-423b-9c88-c1f3169ac646")
                            );
                        }));
            }
        }
        private RelayCommand _rateAppCommand;


        /// <summary>
        /// Sends feedback back to the project email
        /// </summary>
        public RelayCommand SendFeedBackCommand
        {
            get
            {
                return _sendFeedBackCommand
                    ?? (_sendFeedBackCommand = new RelayCommand(
                        async () => {
                            EmailRecipient sentTo = new EmailRecipient() {
                                Address = "uthportal@gmail.com"
                            };

                            EmailMessage mail = new EmailMessage() {
                                Subject = "[UTHPortal-WindowsPhone] Feedback",
                                Body = String.Empty
                            };
                            mail.To.Add(sentTo);

                            await EmailManager.ShowComposeNewEmailAsync(mail);
                        }));
            }
        }
        private RelayCommand _sendFeedBackCommand;
    }
}
