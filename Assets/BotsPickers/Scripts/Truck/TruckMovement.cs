using System;
using System.Collections;
using UnityEngine;

namespace BotsPickers
{
    public class TruckMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;

        private Coroutine _lookCoroutine = null;
        private Coroutine _moveCoroutine = null;

        public event Action Arrived;

        public void OnMove(Vector3 targetPosition, float interactionDistance, ITargeted target)
        {
            if (_lookCoroutine != null || _moveCoroutine != null)
                StopMoving();

            _lookCoroutine = StartCoroutine(Look(targetPosition));
            _moveCoroutine = StartCoroutine(Move(targetPosition, interactionDistance, target));
        }

        public void StopMoving()
        {
            if (_lookCoroutine != null)
                StopCoroutine(_lookCoroutine);

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _lookCoroutine = null;
            _moveCoroutine = null;
        }

        private IEnumerator Look(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            while (transform.rotation != rotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _speed * Time.deltaTime);

                yield return null;
            }

            _lookCoroutine = null;
        }

        private IEnumerator Move(Vector3 targetPosition, float interactionDistance, ITargeted target)
        {
            bool isMoving = true;
            float distance;
            Vector3 moveDirection;
            RaycastHit hit;

            while (isMoving)
            {
                distance = Vector3.Distance(transform.position, targetPosition);

                moveDirection = (targetPosition - transform.position).normalized;
                transform.position += moveDirection * _speed * Time.deltaTime;

                if (distance < interactionDistance)
                    isMoving = false;

                Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);

                if (hit.collider != null)
                    if (hit.collider.GetComponent<ITargeted>() == target)
                        isMoving = false;

                yield return null;
            }

            Arrived?.Invoke();

            _moveCoroutine = null;
        }
    }
}
