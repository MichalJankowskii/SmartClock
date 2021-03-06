﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SmartClock.Core;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.Threading;

namespace SmartClock.UWPRenderer
{
    public class ImageSourceRenderer : IClockRenderer
    {
        public RenderInfo Info => new RenderInfo() { Name = "ImageSourceRenderer", Version = "1.0.0" };
        

        public DataObject Data { get; private set; } = new DataObject();
        
        public async Task RenderAsync(Image<Rgba32> image, CancellationToken token)
        {
            await Data.SetImageSourceAsync(image,token);
        }

        public class DataObject : INotifyPropertyChanged
        {
            TaskScheduler uiSchduler;
            public event PropertyChangedEventHandler PropertyChanged;
            public BitmapSource Image { get; private set; }
            public DataObject()
            {
                uiSchduler = TaskScheduler.FromCurrentSynchronizationContext();
            }
            internal async Task SetImageSourceAsync(Image<Rgba32> source, CancellationToken token)
            {
                token.ThrowIfCancellationRequested();
                if (source==null)
                {
                    return;
                }
                var stream = new MemoryStream();
                source.SaveAsBmp(stream);
                stream.Position = 0;
                token.ThrowIfCancellationRequested();
                await Task.Factory.StartNew(() =>
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(stream.AsRandomAccessStream());
                    Image = bitmap;
                    //Image = await BitmapFactory.FromStream(stream);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
                },Task.Factory.CancellationToken,TaskCreationOptions.None, uiSchduler);
                
            }
        }
    }
}
