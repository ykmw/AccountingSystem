import { OrganisationService } from './organisation.service';

describe('OrganisationService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: OrganisationService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new OrganisationService(httpClientSpy as any);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
 