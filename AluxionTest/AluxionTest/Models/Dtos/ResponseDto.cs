using System;
using System.Collections.Generic;

namespace AluxionTest.Models.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public string DisplayMessage { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
