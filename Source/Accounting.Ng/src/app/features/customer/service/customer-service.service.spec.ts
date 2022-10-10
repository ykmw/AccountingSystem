import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { CustomerServiceService } from './customer-service.service';

describe('CustomerServiceService', () => {
  let service: CustomerServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientModule]
    });
    service = TestBed.inject(CustomerServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
