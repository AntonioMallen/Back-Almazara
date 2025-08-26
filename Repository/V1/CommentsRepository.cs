using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public class CommentsRepository : ICommentsRepository
    {

        public CommentsRepository()
        {
        }
        public RepositoryResult<List<CommentDTO>> Index(int notice_id_i)
        {
            using (var context = new AlmazaraContext())
            {

                var commentsWithUsers = (from comm in context.TComments
                                         join user in context.TUsers on comm.UserIdI equals user.IdI
                                         where comm.NoticeIdI == notice_id_i && comm.DisableB == false
                                         orderby comm.DateDt descending
                                         select new CommentDTO
                                         {
                                             IdI = comm.IdI,
                                             UserIdI = comm.UserIdI,
                                             userNameNv = user.NameNv, 
                                             NoticeIdI = comm.NoticeIdI,
                                             ContentNv = comm.ContentNv,
                                             DateDt = comm.DateDt,
                                             DisableB = comm.DisableB
                                         }).ToList();

                return new RepositoryResult<List<CommentDTO>>() { Success = commentsWithUsers != null && commentsWithUsers.Count > 0, Data = commentsWithUsers };
            }
        }

        public RepositoryResult<TComment> Create(TComment comment)
        {
            using (var context = new AlmazaraContext())
            {
                try
                {
                    context.TComments.Add(comment);
                    context.SaveChanges();
                    return RepositoryResult<TComment>.Ok(comment, "Se ha creado el comentario correctamente.");
                }
                catch (Exception ex)
                {
                    return RepositoryResult<TComment>.Fail($"Error al crear el comentario: {ex.Message}");
                }
            }
        }

        public RepositoryResult<TComment> Edit(TComment comment)
        {
            using (var context = new AlmazaraContext())
            {
                var commentBBDD = context.TComments
                    .Where(comm => comm.IdI == comment.IdI)
                    .FirstOrDefault();

                try
                {

                    if (commentBBDD == null)
                    {
                        commentBBDD = new TComment
                        {
                            IdI = comment.IdI,
                            UserIdI = comment.UserIdI,
                            NoticeIdI = comment.NoticeIdI,
                            ContentNv = comment.ContentNv,
                            DateDt = comment.DateDt,
                            DisableB = false
                        };
                       
                        context.TComments.Add(commentBBDD);

                        context.SaveChanges();
                    }
                    else
                    {
                        commentBBDD.ContentNv = comment.ContentNv;
                        commentBBDD.DateDt = comment.DateDt;

                        context.SaveChanges();
                    }


                    return RepositoryResult<TComment>.Ok(commentBBDD, "Se ha editado el comentario correctamente.");
                }
                catch (Exception ex)
                {
                    return RepositoryResult<TComment>.Fail($"Error al editar el comentario: {ex.Message}");
                }

            }
        }

        public RepositoryResult<TComment> Delete(int comment_id_i)
        {
            using (var context = new AlmazaraContext())
            {
                var comment = context.TComments
                    .Where(comment => comment.IdI == comment_id_i)
                    .FirstOrDefault();

                if (comment != null)
                {
                    comment.DisableB = true;
                    context.SaveChanges();
                    return RepositoryResult<TComment>.Ok(comment, "Se eliminado la noticia correctamente.");
                }
                else
                {
                    return RepositoryResult<TComment>.Fail("No se ha encontrado el comentario.");
                }
            }
        }

    }
}
