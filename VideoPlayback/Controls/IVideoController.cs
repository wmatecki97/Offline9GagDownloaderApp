﻿namespace VideoPlayback.Controls
{
    public interface IVideoController
    {
        VideoStatus Status { get; set; }
        TimeSpan Duration { get; set; }
    }
}
