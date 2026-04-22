import { ApiError } from './api.error';

/**
 * A wrapper around the native fetch API that acts as an interceptor.
 * It globally handles 401 Unauthorized responses by dispatching an event
 * that the AuthService listens to, ensuring the user is logged out.
 */
export const apiClient = async (input: RequestInfo | URL, init?: RequestInit): Promise<Response> => {
  const response = await fetch(input, init);
  
  const url = input.toString();
  const isAuthEndpoint = url.includes('/auth/login') || url.includes('/auth/register');
  
  if (response.status === 401 && !isAuthEndpoint) {
    window.dispatchEvent(new CustomEvent('unauthorized'));
    throw new ApiError(401, 'Session expired. Please log in again.');
  }
  
  return response;
};
