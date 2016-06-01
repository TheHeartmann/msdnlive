using Microsoft.ProjectOxford.Emotion;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Linq;
using EmotionApp.Models;
using System.Reflection;

namespace EmotionApp.MVVM
{
    public class MainPageViewModel : BaseViewModel
    {
        private ImageCapture _imageCapture;
        private readonly EmotionServiceClient _client;
        private bool _canTakePicture;

        private EmotionScore _surprise;
        private EmotionScore _anger;
        private EmotionScore _contemt;
        private EmotionScore _disgust;
        private EmotionScore _fear;
        private EmotionScore _happiness;
        private EmotionScore _neutral;
        private EmotionScore _sadness;

        public MainPageViewModel()
        {
            _imageCapture = new ImageCapture();
            _client = new EmotionServiceClient("[Your API key here]");

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _imageCapture.InitializeAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            CanTakePicture = true;
        }


        private RelayCommand _takePictureCommand { get; set; }


        public ICommand TakePictureCommand
        {
            get
            {
                if (_takePictureCommand == null)
                    _takePictureCommand = new RelayCommand(TakePicture);

                return _takePictureCommand;
            }
        }


        public bool CanTakePicture
        {
            get { return _canTakePicture; }
            set
            {
                if (_canTakePicture == value)
                    return;

                _canTakePicture = value;
                ShoutAbout("CanTakePicture");
            }
        }


        private async void TakePicture()
        {
            CanTakePicture = false;
            var image = await _imageCapture.CaptureJpegImageAsync();
            
            await ProcessImageWithEmotionApiAsync(image);

            CanTakePicture = true;
        }


        private async Task ProcessImageWithEmotionApiAsync(WriteableBitmap image)
        {
            if (image == null)
                return;

            var file       = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("lastImageCapture.jpg", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var fileStream = await file.OpenStreamForReadAsync();
            var results    = await _client.RecognizeAsync(fileStream);

            ParseResults(results, image);
        }


        private void ParseResults(Emotion[] results, WriteableBitmap bitmap)
        {
            if(results == null || results.Length == 0)
                return;

            var scores           = results[0].Scores;
            var properties       = scores.GetType().GetTypeInfo().DeclaredProperties;
            var sortedProperties = properties.Select(s => new { Name = s.Name, Score = s.GetValue(scores) }).OrderByDescending(o => o.Score);
            
            var first = sortedProperties.First();

            switch(first.Name)
            {
                case "Anger" : AngerScore        = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Contempt": ContemptScore   = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Disgust": DisgustScore     = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Fear": FearScore           = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Happiness": HappinessScore = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Neutral": NeutralScore     = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Sadness": SadnessScore     = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
                case "Surprise": SurpriseScore   = new EmotionScore(first.Name, (float)first.Score) { Bitmap = bitmap }; break;
            }
        }


        public EmotionScore ContemptScore
        {
            get { return _contemt; }
            set
            {
                if (_contemt == value)
                    return;

                _contemt = value;
                ShoutAbout("ContemptScore");
            }
        }


        public EmotionScore DisgustScore
        {
            get { return _disgust; }
            set
            {
                if (_disgust == value)
                    return;

                _disgust = value;
                ShoutAbout("DisgustScore");
            }
        }


        public EmotionScore FearScore
        {
            get { return _fear; }
            set
            {
                if (_fear == value)
                    return;

                _fear = value;
                ShoutAbout("FearScore");
            }
        }


        public EmotionScore HappinessScore
        {
            get { return _happiness; }
            set
            {
                if (_happiness == value)
                    return;

                _happiness = value;
                ShoutAbout("HappinessScore");
            }
        }


        public EmotionScore NeutralScore
        {
            get { return _neutral; }
            set
            {
                if (_neutral == value)
                    return;

                _neutral = value;
                ShoutAbout("NeutralScore");
            }
        }


        public EmotionScore SurpriseScore
        {
            get { return _surprise; }
            set
            {
                if (_surprise == value)
                    return;
                _surprise = value;
                ShoutAbout("SurpriseScore");
            }
        }


        public EmotionScore SadnessScore
        {
            get { return _sadness; }
            set
            {
                if (_sadness == value)
                    return;

                _sadness = value;
                ShoutAbout("SadnessScore");
            }
        }


        public EmotionScore AngerScore
        {
            get { return _anger; }
            set
            {
                if (_anger == value)
                    return;

                _anger = value;
                ShoutAbout("AngerScore");
            }
        }
    }
}
