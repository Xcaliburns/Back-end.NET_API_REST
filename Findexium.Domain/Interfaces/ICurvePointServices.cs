﻿using Dot.Net.WebApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface ICurvePointServices
    {
        Task<IEnumerable<CurvePoint>> GetAllAsync();
        Task<CurvePoint> GetByIdAsync(int id);
        Task AddAsync(CurvePoint curvePoint);
        Task UpdateAsync(int id, CurvePoint curvePoint);
        Task DeleteAsync(int id);
    }
}