import { Photo } from './photo';

export interface User {
  id: number;
  userName: string;
  age: number;
  gender: string;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string;
  introduction?: string;
  knownAs?: string;
  lookingFor?: string;
  photos?: Photo[];
  roles?: string[];
}
