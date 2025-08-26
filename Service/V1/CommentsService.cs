using System.Buffers.Text;
using System.Xml.Linq;
using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Repository.V1;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;

namespace Back_Almazara.Service.V1
{
    public class CommentsService : ICommentsService
    {

        private readonly IMapper _mapper;
        private readonly ICommentsRepository _commentEntity;
        private readonly IHashUtility _hashUtility;
        public CommentsService(IMapper mapper, ICommentsRepository commentEntity, IHashUtility hashUtility)
        {
            _mapper = mapper;
            _commentEntity = commentEntity;
            _hashUtility = hashUtility;
        }

        public RepositoryResult<CommentDTO> Create(CommentViewModel comment)
        {

            var userID = (int)_hashUtility.FromBase36(comment.UserIdI ?? "");
            var noticeID = (int)_hashUtility.FromBase36(comment.NoticeIdI ?? "");
            
            var TComment = new TComment
            {
                IdI = 0
                , UserIdI   = userID
                , NoticeIdI = noticeID
                , ContentNv = comment.ContentNv 
                , DateDt    = comment.DateDt    
            };

            return _commentEntity.Create(TComment)
                .MapTo<TComment, CommentDTO>(_mapper);
        }

        public RepositoryResult<CommentDTO> Delete(string comment_id_i)
        {
            var commentID = _hashUtility.FromBase36(comment_id_i);

            return _commentEntity.Delete((int)commentID)
                .MapTo<TComment, CommentDTO>(_mapper);
        }

        public RepositoryResult<CommentDTO> Edit(CommentViewModel comment)
        {
            var commentID = (int)_hashUtility.FromBase36(comment.IdI ?? "");
            var userID = (int)_hashUtility.FromBase36(comment.UserIdI ?? "");
            var noticeID = (int)_hashUtility.FromBase36(comment.NoticeIdI ?? "");

            var commentDTO = new TComment
            {
                IdI = commentID
                , UserIdI   = userID
                , NoticeIdI = noticeID
                , ContentNv = comment.ContentNv 
                , DateDt    = comment.DateDt    
            };

            var resultDTO = _commentEntity.Edit(commentDTO)
                .MapTo<TComment, CommentDTO>(_mapper);

            return resultDTO;
        }

        public RepositoryResult<List<CommentViewModel>> Index(string notice_id_i)
        {
            var noticeID = (int)_hashUtility.FromBase36(notice_id_i ?? "");
            RepositoryResult<List<CommentDTO>> notices = _commentEntity.Index(noticeID);

            if (notices.Success) {
                var comments = notices.Data?
                    .Select(n => {
                        return new CommentViewModel
                        {
                            IdI = _hashUtility.ToBase36(n.IdI),
                            UserIdI = _hashUtility.ToBase36(n.UserIdI),
                            userNameNv = n.userNameNv,
                            NoticeIdI = _hashUtility.ToBase36(n.NoticeIdI),
                            ContentNv = n.ContentNv,
                            DateDt = n.DateDt,
                        };
                    })
                    .ToList();
                return RepositoryResult<List<CommentViewModel>>.Ok(comments);
            }
            if (notices.Data?.Count == 0)
            {
                return RepositoryResult<List<CommentViewModel>>.Ok(new List<CommentViewModel>(),"No hay comentarios");
            }
            return RepositoryResult<List<CommentViewModel>>.Fail("Ha ocurrido un problema");
        }

    }
}
