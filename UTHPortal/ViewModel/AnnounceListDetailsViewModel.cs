using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class AnnounceListDetailsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private Announce _entry;

        /// <summary>
        /// Sets and gets the entry property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Announce entry
        {
            get { return _entry; }
            set { Set(() => entry, ref _entry, value); }
        }

        public AnnounceListDetailsViewModel()
        {
            if (IsInDesignMode)
            {
                Announce newEntry = new Announce();
                newEntry.Title = "Εικονική πόλη για την από κοινού εκπαίδευση";
                newEntry.Plaintext = "Το Πανεπιστήμιο Θεσσαλίας, Τμήμα Ηλεκτρολόγων Μηχανικών και Μηχανικών Υπολογιστών, συμμετέχει στην ανάπτυξη εικονικού περιβάλλοντος πόλης μέσα από το οποίο μαθητές και φοιτητές μπορούν να αναπτύσσουν δεξιότητες σε θετικές επιστήμες, μαθηματικά, και τεχνολογία.  Το ερευνητικό σχέδιο eCity: Virtual City Environment for Engineering Problem-based Learning (http://eCity-project.eu) χρηματοδοτείται με την υποστήριξη του προγράμματος Δια Βίου Μάθησης της Ευρωπαϊκής Επιτροπής. Συντονιστής του σχεδίου είναι το Instituto Superior de Engeharia do Porto της Πορτογαλίας ενώ συμμετέχουν, εκτός από το Πανεπιστήμιο Θεσσαλίας, οργανισμοί από την Ιταλία, Ισπανία, και Τουρκία.\nΠληροφορίες";
                newEntry.Date = new DateTime(2014, 9, 23);
                newEntry.Link = "http://inf.uth.gr/cat=5&par=1234";
                entry = newEntry;
            }
            else
            {
                _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            }
        }

        private RelayCommand _pageLoaded;

        public RelayCommand PageLoaded
        {
            get {
                return _pageLoaded ?? (_pageLoaded = new RelayCommand(() =>
                    {
                        if( _navigationService.StateExists(this.GetType())) {
                            entry = (Announce)_navigationService.GetAndRemoveState(this.GetType());
                        }
                    }));
            }
        }
    }
}
