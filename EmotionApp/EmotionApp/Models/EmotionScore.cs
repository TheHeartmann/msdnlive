using EmotionApp.MVVM;
using Windows.UI.Xaml.Media.Imaging;

namespace EmotionApp.Models
{
    public class EmotionScore : BaseViewModel
    {
        private WriteableBitmap _bitmap;
        private float _score;

        public string Name { get; set; }

        public float Score
        {
            get { return _score; }
            set
            {
                if (_score == value)
                    return;

                _score = value;
                ShoutAbout("Score");
                ShoutAbout("Percentage");
            }
        }


        public EmotionScore(string name, float score)
        {
            Name  = name;
            Score = score;
        }

        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (_bitmap == value)
                    return;

                _bitmap = value;
                ShoutAbout("Bitmap");
            }
        }

        public string Percentage
        {
            get { return string.Format("{0}%", (Score * 100).ToString("0.0")); }
        }
    }
}
