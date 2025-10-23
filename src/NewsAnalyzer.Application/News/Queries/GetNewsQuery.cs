using MediatR;
using NewsAnalyzer.Application.DTO.Responses;

namespace NewsAnalyzer.Application.News.Queries;

public sealed record GetNewsQuery(int Page, int PageSize) : IRequest<List<NewsDetailDto>>;