using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;

namespace Back_Almazara.Service.V1
{
    public interface ICommentsService
    {

        public RepositoryResult<List<CommentViewModel>> Index(string notice_id_i);
        public RepositoryResult<CommentDTO> Create(CommentViewModel notice);
        public RepositoryResult<CommentDTO> Edit(CommentViewModel noticeDetail);
        public RepositoryResult<CommentDTO> Delete(string comment_id_i);
    }
}
