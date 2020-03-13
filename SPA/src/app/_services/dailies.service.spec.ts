import { TestBed } from '@angular/core/testing';

import { DailiesService } from './dailies.service';

describe('DailiesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DailiesService = TestBed.get(DailiesService);
    expect(service).toBeTruthy();
  });
});
