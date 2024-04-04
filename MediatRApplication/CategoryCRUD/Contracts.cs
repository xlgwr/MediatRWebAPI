using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRApplication.CategoryCRUD
{
    public class CreateCategory : IRequest<CreateCategoryResult>
    {
        public string Message { get; set; }
    }
    public class CreateCategoryResult
    {
        public string Message { get; set; }
    }

    public class ReadCategory : TryParseDD<ReadCategory>, IRequest<ReadCategoryResult>
    {
        public string Message { get; set; }
    }
    public class ReadCategoryResult
    {
        public string Message { get; set; }
    }

    public class UpdateCategory : IRequest<UpdateCategoryResult>
    {
        public string Message { get; set; }
    }
    public class UpdateCategoryResult
    {
        public string Message { get; set; }
    }

    public class DeleteCategory : IRequest
    {
        public string Message { get; set; }
    }
}
