import { firstValueFrom } from 'rxjs';
import { taskService } from './services/task.service';
import { authService } from './services/auth.service';
import { redirect } from 'react-router';
import { ApiError } from './services/api.error';

export const tasksLoader = async () => {
  if (!authService.isAuthenticated) return redirect('/login');
  try {
    return await firstValueFrom(taskService.fetchTasks());
  } catch (error) {
    if (error instanceof ApiError) {
      throw new Response(error.message, { status: error.status });
    }
    throw error;
  }
};

export const taskDetailLoader = async ({ params }: any) => {
  if (!authService.isAuthenticated) return redirect('/login');
  const id = parseInt(params.id);
  if (isNaN(id)) throw new Response('Invalid Task ID', { status: 400 });
  try {
    return await firstValueFrom(taskService.getTask(id));
  } catch (error) {
    if (error instanceof ApiError) {
      throw new Response(error.message, { status: error.status });
    }
    throw error;
  }
};
