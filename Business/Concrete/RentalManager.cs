﻿using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            IResult result = BusinessRules.Run(CheckCarExistInRentalList(rental));

            if (result != null)
            {
                return result;
            }

            _rentalDal.Add(rental);
            return new SuccessResult(Messages.AddedRental);
        }

        [SecuredOperation("admin")]
        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.DeletedRental);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll());
        }

        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(I => I.Id == id));
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        {
            return new SuccessDataResult<List<RentalDetailDto>>(_rentalDal.GetCarDetails(), Messages.ReturnedRental);
        }

        public IResult isRentable(Rental rental)
        {
            var result = _rentalDal.GetAll(r => r.CarId == rental.CarId
                    && r.ReturnDate >= rental.RentDate
                    && r.RentDate <= rental.ReturnDate).Any();
            if (result)
                return new ErrorResult();
            return new SuccessResult();

        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.UpdatedRental);
        }

        private IResult CheckCarExistInRentalList(Rental rental)
        {
            if (rental.ReturnDate > DateTime.Now && _rentalDal.GetCarDetails(I => I.CarId == rental.CarId).Count > 0)
            {
                return new ErrorResult(Messages.FailedRentalAddOrUpdate);
            }

            return new SuccessResult();
        }
    }
}
