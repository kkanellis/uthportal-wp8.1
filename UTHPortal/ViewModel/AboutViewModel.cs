using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using UTHPortal.Common;
using Windows.ApplicationModel.Email;
using Windows.Storage;

namespace UTHPortal.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private IDataService _dataService;

        private string _description = "Το UTHPortal ειναι μια ιδέα που σχεδιάστηκε και υλοποιήθηκε απο φοιτητές του Π.Θ έτσι ώστε να προσφέρει στην πανεπιστημιακή κοινότητα. Στόχος της εφαρμογής ειναι η άμεση και εύκολη πρόσβαση σε πληροφορίες που συχνά χρειάζονται οι φοιτητές.\n\n Σε πρώτη φάση η εφαρμογή παρέχει πληροφορίες σχετικά με το πανεπιστήμιο και το τμήμα ΗΜΜΥ/ΤΜΗΥΔ. Πιστεύουμε πως υπάρχει μια πληθώρα δυνατοτήτων που μπορούν να προστεθούν σε μεταγενέστερες εκδόσεις, ωστώσο χρειαζόμαστε και τα δικά σας σχόλια για το μέλλον της εφαρμογής.\n\n Για οποιαδήποτε παρατήρηση/πρόβλημα/πρόταση μπορείτε να επικοινωνήσετε μαζί μας στο uthportal@gmail.com";
        public string Description
        {
            get { return _description; }
        }

        public AboutViewModel()
        {
            if (!IsInDesignMode) {
                _dataService = SimpleIoc.Default.GetInstance<IDataService>();
            }
        }

        private RelayCommand _rateAppCommand;

        /// <summary>
        /// Gets the RateAppCommand.
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
                                                new Uri("zune:reviewapp?appid=appce084dc9-99d5-423b-9c88-c1f3169ac646"));
                                          }));
            }
        }

        private RelayCommand _sendFeedBackCommand;

        /// <summary>
        /// Gets the SendFeedBackCommand.
        /// </summary>
        public RelayCommand SendFeedBackCommand
        {
            get
            {
                return _sendFeedBackCommand
                    ?? (_sendFeedBackCommand = new RelayCommand(
                                          async () =>
                                          {
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
    }
}
