namespace ITJournalDTOs.DTO
{
    public class LikesFilter : RequestFilterData
    {
        public int? LikeId { get; set; }
        public int? ArticleId { get; set; }
        public int? UserId { get; set; }
    }
}
