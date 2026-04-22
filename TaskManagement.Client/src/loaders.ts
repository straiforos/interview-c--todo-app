import { firstValueFrom } from 'rxjs';
import { taskService } from './services/task.service';
import { authService } from './services/auth.service';
import { redirect } from 'react-router';
import { ApiError } from './services/api.error';

export const requirePermission = (permission: string) => {
  if (!authService.isAuthenticated) return redirect('/login');
  if (!authService.hasPermission(permission)) {
    throw new Response('Forbidden: Insufficient Permissions', { status: 403 });
  }
  return null;
};

export const tasksLoader = async () => {
  const authCheck = requirePermission('Tasks.Read');
  if (authCheck) return authCheck;
  
  try {
    return await firstValueFrom(taskService.fetchTasks());
  } catch (error) {
    if (error instanceof ApiError) {
      if (error.status === 401) return redirect('/login');
      throw new Response(error.message, { status: error.status });
    }
    throw error;
  }
};

export const taskDetailLoader = async ({ params }: any) => {
  const authCheck = requirePermission('Tasks.Read');
  if (authCheck) return authCheck;

  const id = parseInt(params.id);
  if (isNaN(id)) throw new Response('Invalid Task ID', { status: 400 });
  try {
    return await firstValueFrom(taskService.getTask(id));
  } catch (error) {
    if (error instanceof ApiError) {
      if (error.status === 401) return redirect('/login');
      throw new Response(error.message, { status: error.status });
    }
    throw error;
  }
};

export const taskCreateLoader = async () => {
  const authCheck = requirePermission('Tasks.Create');
  if (authCheck) return authCheck;
  return null;
};

export const taskEditLoader = async ({ params }: any) => {
  const authCheck = requirePermission('Tasks.Update');
  if (authCheck) return authCheck;

  const id = parseInt(params.id);
  if (isNaN(id)) throw new Response('Invalid Task ID', { status: 400 });
  try {
    return await firstValueFrom(taskService.getTask(id));
  } catch (error) {
    if (error instanceof ApiError) {
      if (error.status === 401) return redirect('/login');
      throw new Response(error.message, { status: error.status });
    }
    throw error;
  }
};
