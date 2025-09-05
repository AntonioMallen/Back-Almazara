using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public class NoticesRepository : INoticesRepository
    {

        public NoticesRepository()
        {
        }

        public RepositoryResult<TNotice> Create(TNotice notice)
        {
            using (var context = new AlmazaraContext())
            {
                try
                {
                    context.TNotices.Add(notice);
                    context.SaveChanges();
                    return RepositoryResult<TNotice>.Ok(notice, "Se ha creado la noticia correctamente.");
                }
                catch (Exception ex)
                {
                    return RepositoryResult<TNotice>.Fail($"Error al crear el notice: {ex.Message}");
                }
            }
        }

        public RepositoryResult<TNotice> Delete(int notice_id_i)
        {
            using (var context = new AlmazaraContext())
            {
                var notice = context.TNotices
                    .Where(notice => notice.IdI == notice_id_i)
                    .FirstOrDefault();

                if (notice != null)
                {
                    notice.DisableB = true;
                    context.SaveChanges();
                    return RepositoryResult<TNotice>.Ok(notice, "Se eliminado la noticia correctamente.");
                }
                else
                {
                    return RepositoryResult<TNotice>.Fail("No se ha encontrado la noticia.");
                }
            }
        }

        public RepositoryResult<NoticeDetailDTO> Detail(int notice_id_i)
        {
            using (var context = new AlmazaraContext())
            {
                var notice = context.TNotices
                    .Where(notice => notice.DisableB == false && notice.IdI == notice_id_i)
                    .FirstOrDefault();

                if (notice != null)
                {
                    var noticeDetail = context.TNoticesDetails
                        .Where(detail => detail.DisableB == false && detail.NoticeIdI == notice.IdI).FirstOrDefault() ?? new TNoticesDetail();


                    var dto = new NoticeDetailDTO
                    {
                        IdI = noticeDetail?.IdI.ToString() ?? "-1" ,
                        NoticeIdI = notice.IdI,
                        NameNv = notice.NameNv,
                        DescriptionNv = notice.DescriptionNv,
                        ImageNv = Convert.ToBase64String(notice.ImageNv),
                        TitleNv = noticeDetail?.TitleNv,
                        SubtitleNv = noticeDetail?.SubtitleNv,
                        ContentNv = noticeDetail?.ContentNv
                    };


                    return RepositoryResult<NoticeDetailDTO>.Ok(dto);
                }

                return RepositoryResult<NoticeDetailDTO>.Fail("No existe la noticia con ese identificador.");
            }
        }

        public RepositoryResult<TNoticesDetail> Edit(NoticeEditDTO noticeDetail)
        {
            using (var context = new AlmazaraContext())
            {
                var noticeBBDD = context.TNotices
                    .Where(not => not.IdI == noticeDetail.IdI)
                    .FirstOrDefault();

                if (noticeBBDD == null)
                {
                    return RepositoryResult<TNoticesDetail>.Fail("No existe la noticia que se busca editar.");
                }

                try
                {
                    noticeBBDD.NameNv = noticeDetail.NameNv;
                    noticeBBDD.DescriptionNv = noticeDetail.DescriptionNv;
                    noticeBBDD.ImageNv = Convert.FromBase64String(noticeDetail.ImageNv);

                    var noticeDetailBBDD = context.TNoticesDetails
                        .Where(notD => notD.NoticeIdI == noticeDetail.IdI)
                        .FirstOrDefault();

                    if (noticeDetailBBDD == null)
                    {
                        noticeDetailBBDD = new TNoticesDetail
                        {
                            NoticeIdI = noticeBBDD.IdI,
                            TitleNv = noticeDetail.TitleNv,
                            SubtitleNv = noticeDetail.SubtitleNv,
                            ContentNv = noticeDetail.ContentNv,
                            DisableB = false
                        };
                        context.TNoticesDetails.Add(noticeDetailBBDD);

                        context.SaveChanges();
                    }
                    else
                    {
                        noticeDetailBBDD.TitleNv = noticeDetail.TitleNv;
                        noticeDetailBBDD.SubtitleNv = noticeDetail.SubtitleNv;
                        noticeDetailBBDD.ContentNv = noticeDetail.ContentNv;

                        context.SaveChanges();
                    }


                    return RepositoryResult<TNoticesDetail>.Ok(noticeDetailBBDD, "Se ha editado la noticia correctamente.");
                }
                catch (Exception ex)
                {
                    return RepositoryResult<TNoticesDetail>.Fail($"Error al editar el notice: {ex.Message}");
                }

            }
        }

        public RepositoryResult<List<NoticeDTO>> Index(int user_id_i)
        {
            using (var context = new AlmazaraContext())
            {
                var notices =
                    (from notice in context.TNotices
                     where notice.DisableB == false
                     join favorite in context.TFavorites
                         on new { NoticeId = notice.IdI, UserId = user_id_i }
                         equals new { NoticeId = favorite.NoticeIdI, UserId = favorite.UserIdI }
                         into favoritesGroup
                     from favorite in favoritesGroup.DefaultIfEmpty()
                     select new NoticeDTO
                     {
                         IdI = notice.IdI.ToString(),
                         NameNv = notice.NameNv,
                         DescriptionNv = notice.DescriptionNv,
                         ImageNv = Convert.ToBase64String(notice.ImageNv),
                         favorite = favorite != null && favorite.DisableB == false
                     }).ToList();


                return new RepositoryResult<List<NoticeDTO>>() { Success = notices != null && notices.Count > 0, Data = notices };
            }
        }

        public RepositoryResult<TFavorite> FavoriteNotice(TFavorite favoriteInfo)
        {
            try
            {
                using (var context = new AlmazaraContext())
                {
                    var favorite = context.TFavorites
                        .Where(favorite => favorite.NoticeIdI == favoriteInfo.NoticeIdI && favorite.UserIdI == favoriteInfo.UserIdI)
                        .FirstOrDefault();

                    if (favorite != null)
                    {
                        favorite.DisableB = !favorite.DisableB;
                        context.SaveChanges();
                        return RepositoryResult<TFavorite>.Ok(favorite);
                    }
                    else // Si nunca le ha dado o quitado favorito se lo pongo
                    {
                        favoriteInfo.DisableB = false;
                        context.TFavorites.Add(favoriteInfo);
                        context.SaveChanges();
                        return RepositoryResult<TFavorite>.Ok(favoriteInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                return RepositoryResult<TFavorite>.Fail($"Error al actualizar favorito: {ex.Message}");
            }
        }

    }
}
